import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LocalStorageManager } from '../utilities/local-storage-manager';
import { Stopwatch } from '../utilities/stopwatch';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Rx';
import 'rxjs/add/observable/forkJoin';

@Component({
  selector: 'app-http-redis',
  templateUrl: './http-redis.component.html'
})
export class HttpRedisComponent {
  public requests: IHttpRequestHistory[];
  public dataRequest: IHttpDataRequest;
  private storageName: string = "htppData";
  private requestNumArray: number[];
  private httpClient: HttpClient;
  private baseUrl: string;
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.httpClient = http;
    this.baseUrl = baseUrl;
    this.dataRequest = new HttpDataRequest();
    this.requests = LocalStorageManager.getLocalStorageArray<IHttpRequestHistory>(this.storageName);
  }
  public makeRequests(dataRequest: IHttpDataRequest) {
    this.requestNumArray = this.createArrayOfRandomNumbers(dataRequest.size);
    let requestHistory = new HttpRequestHistory(dataRequest.size, dataRequest.networkProfile);
    let observables: Observable<IHttpRequestHistory[]>[] = [];
    for (var i = 0; i < dataRequest.numOfRequests; i++) {
      var subject = new Subject();
      let stopwatch = new Stopwatch();
      stopwatch.start();
      const observable = this.sendHttpRequest(stopwatch, requestHistory);
      subject.subscribe(v => function () {
        stopwatch.stop();
        requestHistory.addRequest(stopwatch.elapsed);
      });
      observables.push(observable);
      observable.subscribe(subject);
    }
    Observable.forkJoin(observables).subscribe(() => {
        this.requests.push(requestHistory);
        LocalStorageManager.addToLocalStorageArray(this.storageName, requestHistory);
    });
  }
  private sendHttpRequest(stopwatch: Stopwatch, requestHistory: HttpRequestHistory)  {
    const options = {
      headers: new HttpHeaders().append('numArray', this.requestNumArray.toString()),
    }
    return this.httpClient.get<IHttpRequestHistory[]>(this.baseUrl + 'api/HttpRedis/NonRedisCheck', options);
  }
  private createArrayOfRandomNumbers(size:number): number[] {
    let arrayOfNumbers: number[] = Array.from({ length: size }, () => Math.floor(Math.random() * 10* size));
    return arrayOfNumbers;
  }
}
//.subscribe(result => {
//  var temp = result;
//  stopwatch.stop();
//  requestHistory.addRequest(stopwatch.elapsed);
//}, error => console.error(error));

export class HttpDataRequest implements IHttpDataRequest{
  size: number;
  numOfRequests: number;
  networkProfile: string;
}
interface IHttpDataRequest {
  size: number;
  numOfRequests: number;
  networkProfile: string;
}
class HttpRequestHistory implements IHttpRequestHistory {
  timeStamp: Date;
  maxResponse: number;
  minResponse: number;
  averageResponse: number;
  requestSize: number;
  numRequests: number;
  networkProfile: string;
  private sumOfResponseTime: number;
  constructor(requestSize: number, networkProfile: string) {
    this.timeStamp = new Date();
    this.requestSize = requestSize;
    this.networkProfile = networkProfile;
    this.maxResponse = 0;
    this.minResponse = 0;
    this.numRequests = 0;
    this.sumOfResponseTime = 0;
  }
  public addRequest(elapsedTime: number) {
    this.numRequests++;
    this.sumOfResponseTime += elapsedTime;
    if (elapsedTime > this.maxResponse) this.maxResponse = elapsedTime;
    if (elapsedTime < this.minResponse || this.minResponse === 0) this.minResponse = elapsedTime;
    this.averageResponse = this.sumOfResponseTime / this.numRequests;
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
