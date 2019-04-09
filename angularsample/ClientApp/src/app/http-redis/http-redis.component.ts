import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LocalStorageManager } from '../utilities/local-storage-manager';

@Component({
  selector: 'app-http-redis',
  templateUrl: './http-redis.component.html'
})
export class HttpRedisComponent {
  public requests: IHttpRequestHistory[];
  private storageName: string = "";
  constructor() {
    this.requests = [];
    let cur = new HttpRequestHistory();
    this.requests.push(cur);
    LocalStorageManager.addToLocalStorageArray(this.storageName, new HttpRequestHistory());
  }
/* 
 constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<IHttpRequestHistory[]>(baseUrl + 'api/SampleData/WeatherForecasts').subscribe(result => {
      this.requests = result;
    }, error => console.error(error));
  }
*/
  public addRandomResultsIntoLocalStorage() {
    let cur = new HttpRequestHistory();
    this.requests.push(cur);
    LocalStorageManager.addToLocalStorageArray(this.storageName, new HttpRequestHistory());
  }
}

class HttpRequestHistory implements IHttpRequestHistory {
  timeStamp: Date;
  maxResponse: number;
  minResponse: number;
  averageResponse: number;
  requestSize: number;
  numRequests: number;
  networkProfile: string;
  constructor() {
    this.timeStamp = new Date()
    this.maxResponse= Math.random() * 2
    this.minResponse= Math.random() * 2
    this.averageResponse = Math.random() * 2
    this.requestSize = Math.random() * 2
    this.numRequests = Math.floor(Math.random() * 50)
    this.networkProfile = "none"
  }
}
interface IHttpRequestHistory {
  timeStamp: Date;
  maxResponse: number;
  minResponse: number;
  averageResponse: number;
  requestSize: number;
  numRequests: number;
  networkProfile: string;
}
