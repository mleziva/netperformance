"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var LocalStorageManager = /** @class */ (function () {
    function LocalStorageManager() {
    }
    LocalStorageManager.addToLocalStorageArray = function (name, value) {
        // Get the existing data
        var existing = localStorage.getItem(name);
        // If no existing data, create an array
        // Otherwise, convert the localStorage string to an array
        var arrayContents = existing ? JSON.parse(existing) : [];
        // Add new data to localStorage Array
        arrayContents.push(JSON.stringify(value));
        // Save back to localStorage
        localStorage.setItem(name, JSON.stringify(arrayContents));
    };
    LocalStorageManager.getLocalStorageItem = function (name) {
        var obj = JSON.parse(localStorage.getItem(name));
        return obj;
    };
    LocalStorageManager.getLocalStorageArray = function (name) {
        var arrayOfObjects = [];
        var arrayOfStringItems = JSON.parse(localStorage.getItem(name));
        if (!arrayOfStringItems) {
            return arrayOfObjects;
        }
        for (var i = 0; i < arrayOfStringItems.length; i++) {
            var obj = JSON.parse(arrayOfStringItems[i]);
            arrayOfObjects.push(obj);
        }
        return arrayOfObjects;
    };
    return LocalStorageManager;
}());
exports.LocalStorageManager = LocalStorageManager;
//# sourceMappingURL=local-storage-manager.js.map