import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Like } from '../model/like';

@Injectable({ providedIn: "root" })

export class LikeService {

  baseUrl: string = "api/like/";

  constructor(private http: HttpClient) { }

  addLike(like: Like) {
    return this.http.post(this.baseUrl + "like", like)
  }

  //deleteLike(like: Like) {
  //  return this.http.delete(this.baseUrl + like.reviewId+"/like" )
  //}

  initializeLike(): Like {
    return {
      likeId: 0,
      userId: null,
      reviewId:0
    }
  }
}
