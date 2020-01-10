import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Comment } from '../model/comment';

@Injectable({ providedIn: "root" })
export class CommentService {

  baseUrl: string = "api/comment/";

  constructor(private http: HttpClient) { }

  getCommentList(reviewId: number) {
    return this.http.get(this.baseUrl + reviewId + "/comment");
  }

  addComment(comment: Comment) {
    return this.http.post(this.baseUrl + "comment", comment);
  }

  initializeComment(): Comment {
    return {
      commentId: 0,
      userId: null,
      reviewId: 0,
      commentData: null
    }
  }

}
