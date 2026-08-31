using System;
using System.Collections.Generic;

using UnityEngine;

namespace DeviGames.Playground.MainMenu
{
    public sealed class MissionSelectionView :
        MonoBehaviour
    {
        [SerializeField]
        private MissionSelectionItemView _itemPrefab;

        [SerializeField]
        private Transform _contentRoot;

        private readonly List<MissionSelectionItemView>
            _spawnedItems =
                new();

        public void Show(
            IReadOnlyList<MissionSelectionItem> items,
            Action<string> onPlay)
        {
            Clear();

            foreach (MissionSelectionItem item in items)
            {
                MissionSelectionItemView view =
                    Instantiate(
                        _itemPrefab,
                        _contentRoot);

                view.Bind(
                    item,
                    onPlay);

                _spawnedItems.Add(
                    view);
            }
        }

        private void Clear()
        {
            foreach (MissionSelectionItemView item in
                _spawnedItems)
            {
                Destroy(
                    item.gameObject);
            }

            _spawnedItems.Clear();
        }
    }
}