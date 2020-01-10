import { Injectable } from '@angular/core';

@Injectable()
export class Debuger {
  msg: string;
  component: string;
  method: string;
  variableArray: any;
  constructor() {
    this.msg = "service loaded";
    this.component = "debug";
    this.method = "constructor";
    this.variableArray = [];
    this.produceMessage();
  }
  public produceMessage() {
    console.log("c:", this.component, "--> m:" + this.method, "--> msg:", this.msg, " ", this.variableArray);
  }
  public loadDebuger(c, m, msg, vA) {
    this.msg = msg;
    this.component = c;
    this.method = m;
    this.variableArray = vA;
    this.produceMessage();
  }
}
