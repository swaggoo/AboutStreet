import { AfterViewInit, Component, ElementRef } from '@angular/core';
import { BasketService } from './basket.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss'],
})
export class BasketComponent implements AfterViewInit {
  constructor(public basketService: BasketService, private elementRef: ElementRef) {}

  ngAfterViewInit(): void {
    this.elementRef.nativeElement.ownerDocument
     .body.style.backgroundColor = '#f8f8f8';
  }
}
