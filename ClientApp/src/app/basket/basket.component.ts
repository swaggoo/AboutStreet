import { AfterViewInit, Component, ElementRef } from '@angular/core';
import { BasketService } from './basket.service';
import { BasketItem } from '../shared/models/basket';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss'],
})
export class BasketComponent implements AfterViewInit {
  constructor(
    public basketService: BasketService,
    private elementRef: ElementRef
  ) {}

  ngAfterViewInit(): void {
    this.elementRef.nativeElement.ownerDocument.body.style.backgroundColor =
      '#f8f8f8';
  }

  incrementQuantity(item: BasketItem) {
    this.basketService.addItemToBasket(item);
  }

  removeItem(event: {id: number, quantity: number}) {
    this.basketService.removeItemFromBasket(event.id, event.quantity);
  }
}
