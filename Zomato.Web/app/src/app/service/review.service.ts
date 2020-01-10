import { Injectable } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Review } from '../model/review';

@Injectable({ providedIn: "root" })

export class ReviewService {
  baseUrl: string = "api/review/"

  constructor(private http: HttpClient) { }

  getReviewList(restaurantName: string) {
    return this.http.get(this.baseUrl + restaurantName + "/review");
  }

  getReviewByUser(userId: string) {
    return this.http.get(this.baseUrl + "user/" + userId);
  }

  addNewReview(restaurantName: string, review: Review) {
    return this.http.post(this.baseUrl + restaurantName + "/review", review);
  }

  deleteReview(reviewId: number) {
    return this.http.delete(this.baseUrl + reviewId);
  }

  initializeReview(): Review {
    return {
      restaurantId: 0,
      reviewData: null,
      reviewId: 0,
      userId: null,
      rating: 0
    }
  }
}
