using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;
        private readonly int _levelCount = 20;
        public readonly List<Level> Levels = new();
        private Level _currentlevel;
        private void Awake()
        {
            Instance = this;
            ReadFile();
        }

        private void ReadFile()
        {
            for (var i = 0; i < _levelCount; i++)
            {
                var path = "Assets/Resources/Level" + (i + 1);
                var levelInfo = File.ReadAllText(path);
                Levels.Add(ParseLevel(levelInfo));
            }
        }

        private Level ParseLevel(string levelInfo)
        {
            var level = new Level();
            var splitLevelInfo = levelInfo.Split("\n");
            level.levelNumber = int.Parse(splitLevelInfo[0].Trim().Split(" ").Last());
            level.gridWidth = int.Parse(splitLevelInfo[1].Trim().Split(" ").Last());
            level.gridHeight = int.Parse(splitLevelInfo[2].Trim().Split(" ").Last());
            level.moveCount = int.Parse(splitLevelInfo[3].Trim().Split(" ").Last());
            level.gridLayout = RemoveCharacter(splitLevelInfo[4].Trim().Split(" ").Last(), ',');
            return level;
        }
        private string RemoveCharacter(string line, char character)
        {
            return string.Concat(line.Split(character));
        }

        public Level GetCurrentLevel()
        {
            return _currentlevel;
        }

        public void SetCurrentLevel(Level level)
        {
            _currentlevel = level;
        }
    }
}