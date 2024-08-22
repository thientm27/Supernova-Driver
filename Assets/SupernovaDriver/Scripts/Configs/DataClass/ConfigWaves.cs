using System.Collections.Generic;
using System.Linq;

namespace DuckSurvivor.Scripts.Configs.DataClass
{
    public class ConfigWaves : SgConfigDataTable<ConfigWavesRecord>
    {
        protected override void RebuildIndex()
        {
            RebuildIndexByField<int>("Id");
        }

        public ConfigWavesRecord GetConfigWaveById(int id)
        {
            ConfigWavesRecord record = Records.FirstOrDefault(x => x.Id == id);
            return record;
        }

        public List<ConfigWavesRecord> GetAllConfigWaves()
        {
            return Records;
        }
    }

    public class ConfigWavesRecord
    {
        public int Id;
        public int Amounts;
        public int InitialEnemySpawn;
        public int InitialTimeToSpawnEnemies;
        public int InitialSpawnAmount;
        public bool IsBossWave;
    }
}

