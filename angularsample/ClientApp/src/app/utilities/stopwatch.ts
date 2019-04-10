export class Stopwatch  {

  public timestart: number;
  public elapsed: number;
  constructor() {
    
  }

  public start() {
    this.timestart = new Date().getTime();
  }
  public stop() {
    this.elapsed = (new Date().getTime() - this.timestart);
  }
  public clear() {
    this.timestart = null;
    this.elapsed = null;
  }
}
