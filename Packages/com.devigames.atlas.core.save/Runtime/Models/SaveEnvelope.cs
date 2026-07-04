using System;

namespace DeviGames.Atlas.Core.Save.Models
{
    [Serializable]
    public sealed class SaveEnvelope<T>
    {
        public int Version;
        public string LastModifiedUtc;
        public T Data;

        public SaveEnvelope(int version, T data)
        {
            Version = version;
            LastModifiedUtc = DateTime.UtcNow.ToString("O");
            Data = data;
        }
    }
}