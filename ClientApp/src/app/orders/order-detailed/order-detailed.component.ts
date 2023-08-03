import { Component, OnInit } from '@angular/core';
import { OrderService } from '../order.service';
import { ActivatedRoute } from '@angular/router';
import { Order } from 'src/app/shared/models/order';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-order-detailed',
  templateUrl: './order-detailed.component.html',
  styleUrls: ['./order-detailed.component.scss'],
})
export class OrderDetailedComponent implements OnInit {
  orderId = this.route.snapshot.paramMap.get('id');
  order?: Order;

  constructor(
    private orderService: OrderService,
    private route: ActivatedRoute,
    private breadcrumbService: BreadcrumbService
  ) {
    breadcrumbService.set('@orderDetailed', 'Order #' + this.orderId);
  }

  ngOnInit(): void {
    this.loadOrderDetails();
  }

  loadOrderDetails() {
    if (!this.orderId) return;

    this.orderService.getUserOrder(+this.orderId).subscribe({
      next: (order) => {
        this.order = order;
        console.log(this.order);
      },
    });
  }
}
