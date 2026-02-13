
using NUnit.Framework.Constraints;
using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Assertions;

[ScriptedImporter(1, "yapb")]
public class YapImporter : ScriptedImporter
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	struct YapFileHeader
	{
		public uint MagicValue;
		public int Version;
		public int SceneCount;
	}
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	struct YapFileScene
	{
		public int SceneNameLenght;
		public int ChildCount;
	}
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	struct YapFileLeaf
	{
		public int ContentLenght;
		public int TransitionCount;
		public YapFileLeafType LeafType;
		public CommandType Command; 
	}


	public override void OnImportAsset(AssetImportContext ctx)
	{
		
		YapDataContainer container = ScriptableObject.CreateInstance<YapDataContainer>();
		container.Source = ctx.assetPath;

		ctx.AddObjectToAsset("rootContainer", container);
		ctx.SetMainObject(container);
		FileStream fileHandle = null;
		try
		{
			byte[] buffer = new byte[2048];
			fileHandle = File.OpenRead(ctx.assetPath);
			YapFileHeader header = ReadStruct<YapFileHeader>(fileHandle, buffer);

			Assert.AreEqual(FileHeaderCode('Y', 'A', 'P', '!'), header.MagicValue);

			container.Version = header.Version;
			container.SceneCount = header.SceneCount;

			for (int i = 0; i < header.SceneCount; i++)
			{
				YapSceneData scene = ScriptableObject.CreateInstance<YapSceneData>();

				YapFileScene sceneHeader = ReadStruct<YapFileScene>(fileHandle, buffer);
				scene.SceneName = ReadString(fileHandle, sceneHeader.SceneNameLenght, buffer);

				scene.name = scene.SceneName;
				ctx.AddObjectToAsset($"scene_{i}", scene);

				scene.Lines = new NodeData[sceneHeader.ChildCount];
				
				for (int l = 0; l < sceneHeader.ChildCount; l++)
				{
					YapFileLeaf leafHeader = ReadStruct<YapFileLeaf>(fileHandle, buffer);
					NodeData lineData = new NodeData();
					lineData.LeafType = leafHeader.LeafType;
					lineData.Command = leafHeader.Command;
					lineData.Transitions = new int[leafHeader.TransitionCount];
					for (int t = 0; t < leafHeader.TransitionCount; t++)
					{
						lineData.Transitions[t] = ReadInt(fileHandle, buffer);
						if (lineData.Transitions[t] > 100000)
						{
							throw new Exception("busted transitions");
						}
					}
					lineData.Content = ReadString(fileHandle, leafHeader.ContentLenght, buffer);
					scene.Lines[l] = lineData;
				}				
			}
		}
		catch (Exception e)
		{
			Debug.LogException(e);
			throw e;
		}
		finally
		{
			if (fileHandle != null)
			{
				fileHandle.Close();
			}
		}
	}

	private uint FileHeaderCode(char a, char b, char c, char d)
	{
		return ((uint)a << 0) | ((uint)b << 8) | ((uint)c << 16) | ((uint)d << 24);
	}

	private T ReadStruct<T>(FileStream fileStream, byte[] buffer) where T : struct
	{
		int size = Marshal.SizeOf(typeof(T));
		Assert.IsTrue(buffer.Length >= size);
		int read = fileStream.Read(buffer, 0, size);
		Assert.AreEqual(read, size);

		return ByteArrayToStruct<T>(buffer);
	}

	private string ReadString(FileStream fileStream, int lenght, byte[] buffer)
	{		
		Assert.IsTrue(buffer.Length >= lenght);
		fileStream.Read(buffer, 0, lenght);
		return System.Text.Encoding.UTF8.GetString(buffer, 0, lenght);
	}

	private int ReadInt(FileStream fileStream, byte[] buffer)
	{
		Assert.IsTrue(buffer.Length >= 4);
		fileStream.Read(buffer, 0, 4);
		uint asUint = ((uint)buffer[0] << 0) | ((uint)buffer[1] << 8) | ((uint)buffer[2] << 16) | ((uint)buffer[3] << 24);
		return (int)asUint;
	}

	private T ByteArrayToStruct<T>(byte[] byteArray) where T : struct
	{
		GCHandle pinnedPacket = GCHandle.Alloc(byteArray, GCHandleType.Pinned);
		T result = (T)Marshal.PtrToStructure(
			pinnedPacket.AddrOfPinnedObject(),
			typeof(T));
		pinnedPacket.Free();
		return result;
	}
}
