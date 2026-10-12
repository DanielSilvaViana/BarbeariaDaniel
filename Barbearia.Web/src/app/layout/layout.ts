import { Component } from '@angular/core';
import { Sidebar } from './sidebar/sidebar';
import { Header } from './header/header';

@Component({
  selector: 'app-layout',
  imports: [
    Sidebar,
    Header
  ],
  templateUrl: './layout.html',
  styleUrl: './layout.scss'
})
export class Layout {

}