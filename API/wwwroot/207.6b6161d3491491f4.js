"use strict";(self.webpackChunkClientApp=self.webpackChunkClientApp||[]).push([[207],{9207:(C,u,o)=>{o.r(u),o.d(u,{BasketModule:()=>y});var r=o(6895),c=o(9838),t=o(1571),d=o(5866),l=o(5053),p=o(8795);function k(e,s){1&e&&(t.TgZ(0,"div")(1,"p"),t._uU(2,"There are no products in your items"),t.qZA()())}function f(e,s){if(1&e){const n=t.EpF();t.ynx(0),t.TgZ(1,"div",2)(2,"div",3)(3,"app-basket-summary",4),t.NdJ("addItem",function(m){t.CHM(n);const i=t.oxw();return t.KtG(i.incrementQuantity(m))})("removeItem",function(m){t.CHM(n);const i=t.oxw();return t.KtG(i.removeItem(m))}),t.qZA()(),t.TgZ(4,"div",3)(5,"div",5),t._UZ(6,"app-order-totals"),t.TgZ(7,"div",6)(8,"a",7),t._uU(9," Proceed to checkout "),t.qZA()()()()(),t.BQk()}}const v=[{path:"",component:(()=>{class e{constructor(n,a){this.basketService=n,this.elementRef=a}ngAfterViewInit(){this.elementRef.nativeElement.ownerDocument.body.style.backgroundColor="#f8f8f8"}incrementQuantity(n){this.basketService.addItemToBasket(n)}removeItem(n){this.basketService.removeItemFromBasket(n.id,n.quantity)}}return e.\u0275fac=function(n){return new(n||e)(t.Y36(d.v),t.Y36(t.SBq))},e.\u0275cmp=t.Xpm({type:e,selectors:[["app-basket"]],decls:5,vars:6,consts:[[1,"container","mb-5"],[4,"ngIf"],[1,"container"],[1,"row"],[3,"addItem","removeItem"],[1,"col-6","offset-6"],[1,"d-grid"],["routerLink","/checkout",1,"btn","btn-success","py-2"]],template:function(n,a){1&n&&(t.TgZ(0,"div",0),t.YNc(1,k,3,0,"div",1),t.ALo(2,"async"),t.YNc(3,f,10,0,"ng-container",1),t.ALo(4,"async"),t.qZA()),2&n&&(t.xp6(1),t.Q6J("ngIf",null===t.lcZ(2,2,a.basketService.basketSourse$)),t.xp6(2),t.Q6J("ngIf",t.lcZ(4,4,a.basketService.basketSourse$)))},dependencies:[r.O5,c.rH,l.S,p.b,r.Ov],styles:["tr[_ngcontent-%COMP%]   td[_ngcontent-%COMP%], th[_ngcontent-%COMP%]{background-color:#f8f8f8}"]}),e})()}];let B=(()=>{class e{}return e.\u0275fac=function(n){return new(n||e)},e.\u0275mod=t.oAB({type:e}),e.\u0275inj=t.cJS({imports:[c.Bz.forChild(v),c.Bz]}),e})();var g=o(4466);let y=(()=>{class e{}return e.\u0275fac=function(n){return new(n||e)},e.\u0275mod=t.oAB({type:e}),e.\u0275inj=t.cJS({imports:[r.ez,B,g.m]}),e})()}}]);