@Angular CheatSheet

A component consists of 3 things
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
A component class - Handles data and functionality
An HTML template - Determines the UI
Component specific styles - CSS stylesheet for each component

Angular CLI
----------------------
ng g c {{name}} - generates a new angualar component name is the name you would call it, you can also tack on --skip-tests to not generate any test

Some Basic components 
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
<app-root> - The first component to load and the container for other components

Some functions we can use on the front end HTML
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Interpolotion {{}}
-------------------
We can use interpolation to render the property as text
let name = "Kyle"
<h3>{{name}}</h3> - would show Kyle

Property binding[]
---------------------
To bind to an elements property, enclose it in square brackets, which identifies the property as a target property 

<img alt="UserPhoto" [src]="user.photoUrl">

*ngFor
---------------------
We can use this as a for loop for a list we have in a component, so like in a div or a list 

<div *ngFor="let product of products">{{product.id}}</div>

*ngIf
---------------------
We can use this to test if something exists or is true or false

<p *ngIf="product.description">
    Description: {{ product.description }}
</p>

In the above only if there is a product.description then it will be displayed, so if you loop through a list of objects some may not have a description

Event Binding ()
----------------------
<button type="button" (click)="share()">
	Share
</button>

binds the click event to a share method in the .ts component

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

Pass Data to a child component
//This example will use 3 files, The first Products.ts holds the data, Product-list is the parent, 
===========================

//Products.ts
export interface Product {
  id: number;
  name: string;
  price: number;
  description: string;
}

export const products = [
  {
    id: 1,
    name: 'Phone XL',
    price: 799,
    description: 'A large phone with one of the best screens'
  },
  {
    id: 2,
    name: 'Phone Mini',
    price: 699,
    description: 'A great phone with one of the best cameras'
  },
  {
    id: 3,
    name: 'Phone Standard',
    price: 299,
    description: ''
  }
];

import { Input } from '@angular/core'; //first import input from core
import { Product } from '../products'; //Import out Model/Data

//Include the @Input decorator in the component.ts, this indicates the property value passes in from the components parent
export class ProductAlertsComponent {
@Input() product!: product

}