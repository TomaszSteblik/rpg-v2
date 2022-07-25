using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using rpg_v2;
using System.IO.Compression;
using CompressionMode = System.IO.Compression.CompressionMode;

namespace game.GameEngine
{
    public static class SaveManager 
    {
        public static void SaveGame(string path)
        {
            var serialized = SerializeEntitiesToJson();
            CompressAndSaveJsonToGzip(serialized, path);
        }

        private static void CompressAndSaveJsonToGzip(string serialized, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Empty save dir path");
            
            if (File.Exists(path))
                File.Delete(path);
            
            using var compressedFileStream = File.Create(path);
            using var compressor = new GZipStream(compressedFileStream, CompressionMode.Compress);
            compressor.Write(Encoding.UTF8.GetBytes(serialized));
            compressor.Flush();
        }

        private static string SerializeEntitiesToJson()
        {
            var entities = EcsManager.GetAllEntities();
            var serialized = JsonSerializer.Serialize(entities, new JsonSerializerOptions()
            {
                WriteIndented = true
            });
            return serialized;
        }

        public static void LoadGame(string saveFile)
        {
            var json = ReadJsonFromGzip(saveFile);
            var entities = DeserializeEntitiesFromJson(json);
            LoadEntitiesAndPlayer(entities);
        }

        private static string ReadJsonFromGzip(string saveFile)
        {
            using var compressedFileStream = File.Open(saveFile, FileMode.Open);
            using var decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress);
            using var memoryStream = new StreamReader(decompressor);
            return memoryStream.ReadToEnd();
        }

        private static void LoadEntitiesAndPlayer(ICollection<Entity> entities)
        {
            var player = entities.First(x => x.Mask[2]);
            MainGame.PlayerEntity = player;
            foreach (var entity in entities)
            {
                EcsManager.LoadEntity(entity);
            }
        }

        private static ICollection<Entity> DeserializeEntitiesFromJson(string json)
        {
            var entitiesFromJson = JsonSerializer.Deserialize<ICollection<Entity>>(json, new JsonSerializerOptions());
            if (entitiesFromJson is null)
                throw new DataException("Failed to load save file entities");
            return entitiesFromJson;
        }
    }
}