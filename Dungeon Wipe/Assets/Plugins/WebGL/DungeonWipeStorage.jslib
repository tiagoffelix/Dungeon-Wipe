mergeInto(LibraryManager.library, {
  DungeonWipeSyncFileSystem: function () {
    try {
      FS.syncfs(false, function (error) {
        if (error) {
          console.error('[Dungeon Wipe] Saving to browser storage failed:', error);
        }
      });
    } catch (error) {
      console.error('[Dungeon Wipe] Browser storage is unavailable:', error);
    }
  }
});
