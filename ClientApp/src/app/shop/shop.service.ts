import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Product } from '../shared/models/product';
import { Pagination } from '../shared/models/pagination';
import { Brand } from '../shared/models/brand';
import { Type } from '../shared/models/type';
import { ShopParams } from '../shared/models/shopParams';
import { environment } from 'src/environments/environment';
import { map, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  baseUrl = environment.apiUrl;
  products: Product[] = [];
  brands: Brand[] = [];
  types: Type[] = [];
  pagination?: Pagination<Product[]>;
  shopParams = new ShopParams();
  productCache = new Map<string, Pagination<Product[]>>();

  constructor(private http: HttpClient) {}

  getProducts(useCache = true) {
    if (!useCache) this.productCache = new Map();

    if (this.productCache.size > 0 && useCache) {
      if (this.productCache.has(Object.values(this.shopParams).join('-'))) {
        this.pagination = this.productCache.get(
          Object.values(this.shopParams).join('-')
        );
        if (this.pagination) return of(this.pagination);
      }
    }

    let params = new HttpParams();

    if (this.shopParams.brandId > 0)
      params = params.append('brandId', this.shopParams.brandId);
    if (this.shopParams.typeId > 0)
      params = params.append('typeId', this.shopParams.typeId);
    if (this.shopParams.search.length > 0)
      params = params.append('search', this.shopParams.search);
    params = params.append('sort', this.shopParams.sort);

    params = params.append('pageNumber', this.shopParams.pageNumber);
    params = params.append('pageSize', this.shopParams.pageSize);

    return this.http
      .get<Pagination<Product[]>>(this.baseUrl + 'products', { params })
      .pipe(
        map((response) => {
          this.productCache.set(
            Object.values(this.shopParams).join('-'),
            response
          );
          this.pagination = response;
          return response;
        })
      );
  }

  setShopParams(params: ShopParams) {
    this.shopParams = params;
  }

  getShopParams() {
    return this.shopParams;
  }

  getProduct(id: number) {
    const cachedProduct = this.findProductInCache(id);

    if (cachedProduct) {
      console.log(cachedProduct);
      return of(cachedProduct);
    }

    return this.http.get<Product>(this.baseUrl + 'products/' + id);
  }

  getBrands() {
    if (this.brands.length > 0) return of(this.brands);

    return this.http.get<Brand[]>(this.baseUrl + 'products/brands').pipe(
      map((brands) => {
        this.brands = brands;
        return brands;
      })
    );
  }

  getTypes() {
    if (this.types.length > 0) return of(this.types);

    return this.http.get<Type[]>(this.baseUrl + 'products/types').pipe(
      map((types) => {
        this.types = types;
        return types;
      })
    );
  }

  private findProductInCache(id: number) {
    for (const obj of this.productCache.values()) {
      const product = obj.data.find(product => product.id === id);
      if (product) {
        return product;
      }
    }
    return;
  }
}
