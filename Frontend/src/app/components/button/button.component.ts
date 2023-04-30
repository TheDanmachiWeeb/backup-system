import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.scss'],
})
export class ButtonComponent {
  @Input() variant: 'primary' | 'danger' | 'info' | 'warning' | 'success' =
    'primary';
  @Input() size: 'lg' | 'md' | 'sm' = 'lg';
  @Input() disabled: boolean = false;
  @Input() link: string;

  getClass(): string {
    let className: string = 'rounded-md text-white';

    switch (this.variant) {
      case 'primary':
        className += ' bg-purple-600 hover:bg-purple-700 font-bold';
        break;
      case 'success':
        className += ' bg-green-500 hover:bg-green-600';
        break;
      case 'danger':
        className += ' bg-red-500 hover:bg-red-600';
        break;
      case 'info':
        className += ' bg-blue-500 hover:bg-blue-600';
        break;
      case 'warning':
        className += '';
        break;
    }

    switch (this.size) {
      case 'lg':
        className += ' px-6 h-10';
        break;
      case 'md':
        className += ' px-4 h-9';
        break;
      case 'sm':
        className += ' px-3 h-8';
        break;
    }

    return className;
  }
}
