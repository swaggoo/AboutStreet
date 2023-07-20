import { AfterViewInit, Component, ElementRef, Input, OnInit } from '@angular/core';
import { ShopService } from '../shop.service';
import { Product } from 'src/app/shared/models/product';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss'],
})
export class ProductDetailsComponent implements OnInit, AfterViewInit {
  product?: Product;

  constructor(
    private shopService: ShopService,
    private activatedRoute: ActivatedRoute,
    private elementRef: ElementRef,
    private bcService: BreadcrumbService
  ) {}

  ngAfterViewInit(): void {
    this.elementRef.nativeElement.ownerDocument.body.style.backgroundColor =
      '#f8f8f8';
  }

  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct() {
    const productId = this.activatedRoute.snapshot.paramMap.get('id');

    if (productId) {
      this.shopService.getProduct(+productId).subscribe({
        next: (product) => {
          this.product = product;
          this.bcService.set('@productDetails', product.name);
        }
      });
    }
  }
}
