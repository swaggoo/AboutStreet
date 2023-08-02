import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DeliveryMethod } from '../shared/models/deliveryMethod';
import { environment } from 'src/environments/environment.development';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getDeliveryMethods() {
    return this.http.get<DeliveryMethod[]>(this.baseUrl + 'orders/deliveryMethods').pipe(
      map(dm => {
        return dm.sort((a, b) => b.price - a.price)
      })
    );
  }
}
