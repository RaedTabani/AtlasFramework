using UnityEngine;
using NUnit.Framework;
using DeviGames.Atlas.Core.Events;
using DeviGames.Atlas.Gameplay.Events;
using DeviGames.Atlas.Gameplay.Inventory.Events;
using DeviGames.Atlas.Gameplay.Inventory.Services;
using DeviGames.Atlas.Gameplay.Inventory.Models;

namespace DeviGames.Atlas.Gameplay.Inventory.Services
{
    public class InventoryServiceTests
    {
        private int _addedCount;
        private int _removedCount;
        private InventoryService _inventory;
        [SetUp]
        public void Setup()
        {
            _addedCount = 0;
            _removedCount = 0;
            _inventory = new InventoryService();
            _inventory.Initialize();

            EventBus.Subscribe<ItemAddedToInventoryEvent>(OnItemAdded);
            EventBus.Subscribe<ItemRemovedFromInventoryEvent>(OnItemRemoved);
            
        }

        [TearDown]
        public void TearDown()
        {
            EventBus.Unsubscribe<ItemAddedToInventoryEvent>(OnItemAdded);
            EventBus.Unsubscribe<ItemRemovedFromInventoryEvent>(OnItemRemoved);
            _inventory.Shutdown();
            
            EventBusTestUtility.Reset();
        }


        [Test]
        public void Collecting_Same_Item_Twice_Should_Not_Duplicate()
        {
            EventBus.Publish(new ItemCollectedEvent("golden_key"));

            EventBus.Publish(new ItemCollectedEvent("golden_key"));

            Assert.AreEqual(1,_inventory.ItemCount);
        }

        [Test]
        public void Duplicate_Item_Should_Not_Publish_Twice()
        {
            EventBus.Publish(new ItemCollectedEvent("golden_key"));

            EventBus.Publish(new ItemCollectedEvent("golden_key"));

            Assert.AreEqual(1,_addedCount);
        }

        [Test]
        public void Remove_Should_Remove_Item()
        {
            EventBus.Publish(new ItemCollectedEvent("golden_key"));

            bool removed =_inventory.Remove("golden_key");

            Assert.IsTrue(removed);

            Assert.IsFalse(_inventory.Contains("golden_key"));
        }


        [Test]
        public void Snapshot_Should_Copy_Data()
        {
            EventBus.Publish(new ItemCollectedEvent("golden_key"));

            InventoryData snapshot =_inventory.CreateSnapshot();

            Assert.IsTrue(snapshot.Contains("golden_key"));
        }
        private void OnItemAdded(ItemAddedToInventoryEvent e)
        {
            _addedCount++;
        }
            private void OnItemRemoved(ItemRemovedFromInventoryEvent e)
        {
            _removedCount++;
        }
        
    }
}