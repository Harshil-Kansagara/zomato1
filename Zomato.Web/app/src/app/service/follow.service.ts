import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Follow } from '../model/follow';

@Injectable({ providedIn: "root" })
export class FollowService {

  baseUrl: string = "api/follow/";

  constructor(private http: HttpClient) { }

  getFollowers(userId: string) {
    return this.http.get(this.baseUrl + userId + "/follower");
  }

  getFollowing(userId: string) {
    return this.http.get(this.baseUrl + userId + "/following");
  }

  addFollower(follow: Follow) {
    return this.http.post(this.baseUrl, follow);
  }

  deleteFollower(followerId: string) {
    return this.http.delete(this.baseUrl + followerId);
  }

  initializeFollow(): Follow {
    return {
      id: 0,
      userId: null,
      followingId: null
    }
  }
}
