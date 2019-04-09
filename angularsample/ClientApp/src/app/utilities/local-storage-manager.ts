export abstract class LocalStorageManager {

  public static addToLocalStorageArray(name:string, value:object) {
    // Get the existing data
    let existing = localStorage.getItem(name);
    // If no existing data, create an array
    // Otherwise, convert the localStorage string to an array
    let arrayContents = existing ? existing.split(',') : [];
    // Add new data to localStorage Array
    arrayContents.push(JSON.stringify(value));
    // Save back to localStorage
    localStorage.setItem(name, arrayContents.toString());
  }
  public static getLocalStorageItem<T>(name: string, ): T {
    let obj: T = JSON.parse(localStorage.getItem(name));
    return obj;
  }
  public static getLocalStorageArray<T>(name: string, ): T[] {
    let arrayOfObjects: T[] = [];
    let arrayOfStringItems = localStorage.getItem(name).split(',');
    for (let i = 0; i < arrayOfStringItems.length; i++) {
      let obj: T = JSON.parse(arrayOfStringItems[i]);
      arrayOfObjects.push(obj);
    }
    return arrayOfObjects;
  }
}
