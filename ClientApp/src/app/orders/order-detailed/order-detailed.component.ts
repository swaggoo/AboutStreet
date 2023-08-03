import { Component, OnInit } from '@angular/core';
import { OrderService } from '../order.service';
import { ActivatedRoute } from '@angular/router';
import { Order } from 'src/app/shared/models/order';

@Component({
  selector: 'app-order-detailed',
  templateUrl: './order-detailed.component.html',
  styleUrls: ['./order-detailed.component.scss']
})
export class OrderDetailedComponent implements OnInit{
  order?: Order;

  constructor(private orderService: OrderService, private route: ActivatedRoute) {}
  
  ngOnInit(): void {
    this.loadOrderDetails();
  }

  loadOrderDetails() {
    const orderId = this.route.snapshot.paramMap.get('id');
    if (!orderId) return;

    this.orderService.getUserOrder(+orderId).subscribe({
      next: order => {
        this.order = order;
        console.log(this.order);
      }
    });
  }

}
