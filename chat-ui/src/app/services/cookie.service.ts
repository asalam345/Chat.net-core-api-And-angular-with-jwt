import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CookieService {
  constructor() {
      // this.isConsented = this.getCookie('token') === '1';
  }
  // private isConsented = false;
  private exValue = '';
   cookieProducts: any[] = [];
  cookieIsExist(name: string){
    const gc = this.getCookie(name);
    if (gc !== null && gc !== '') {
       return true;
     }
    return false;
   }
   getAllCart(){
     // tslint:disable-next-line: no-debugger
    const ca: Array<string> = document.cookie.split(';');
    const caLen: number = ca.length;
    let c: string;
    this.cookieProducts = [];
    for (let i = 0; i < caLen; i += 1) {
        c = ca[i].replace(/^\s+/g, '');
        if (c.substring(0, 14) === 'HAL-Mart-Cart-') {
          const productAndQuantity = c.split('=');
          const productId = productAndQuantity[0].substring(14, productAndQuantity[0].length);
          const qty = productAndQuantity[1].substring(0, productAndQuantity[1].length);
          if (qty !== '') {
          this.cookieProducts.push(
            {
              productId, qty
          });
          }
          // this.cookieProducts.quantity.push(productAndQuantity[1].substring(0, productAndQuantity[1].length));
        }
    }
    return this.cookieProducts;
   }
   getCookie(name: string) {
    // debugger;
    const ca: Array<string> = document.cookie.split(';');
    const caLen: number = ca.length;
    const cookieName = `${name}=`;
    let c: string;

    for (let i = 0; i < caLen; i += 1) {
        c = ca[i].replace(/^\s+/g, '');
        if (c.indexOf(cookieName) === 0) {
            return c.substring(cookieName.length, c.length);
        }
    }
    return '';
}
valueIsExistInCookie(name: string, value: string){
  const gc = this.getCookie(name);
  const cs = gc.split('=,.');
  let result = false;
  cs.forEach(element => {
    if (+element.trim() === +value){
      result = true;
    }
  });
  return result;
}
 deleteCookie(name) {
    this.setCookie(name, '', -365, '/');
}
public deleteAllCookies() {
  // clear all cookies
  const cookies = document.cookie.split(';');
  // tslint:disable-next-line: prefer-for-of
  for (let i = 0; i < cookies.length; i++) {
      const cookie = cookies[i];
      const eqPos = cookie.indexOf('=');
      const name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
      document.cookie = name + '=;expires=Thu, 01 Jan 1970 00:00:00 GMT';
  }
}

  public setCookie(name: string, value: string, expireDays: number, path: string = '', stockQuantity: string = '') {
      const d: Date = new Date();
      this.exValue = '0';
      if (this.cookieIsExist(name)){
        this.exValue = this.getCookie(name);
      }
      let expires = expireDays.toString();
      if (expireDays > 0){
        d.setTime(d.getTime() + expireDays * 24 * 60 * 60 * 1000);
        expires = `expires=${d.toUTCString()}`;
        if (stockQuantity !== '' && +this.exValue === +stockQuantity){
          alert(name + ' not available!');
          return;
        }
        if (value === ''){
          value = (+this.exValue + 1).toString();
        }
        else{
          value = value.toString();
        }
      }
      document.cookie = name + '=' + (value || '') + ';' + expires + '; path=/';
  }
}
