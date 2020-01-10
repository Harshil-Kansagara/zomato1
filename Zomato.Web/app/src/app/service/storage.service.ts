import { Injectable } from "@angular/core";

@Injectable({ providedIn: "root" })
export class LocalStorageService {
  public get(): Storage {
    return localStorage;
  }
}
