import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.scss']
})
export class ButtonComponent {
  @Input() text: string;
  @Input() variant: 'primary' | 'secondary' | 'tertiary' = 'primary';
  @Input() disabled: boolean = false;
  @Input() link: string;

  getClass(): string {
    let className: string = "h-10 rounded-md px-6";
    switch (this.variant) {
      case "primary":
        className += ' bg-cyany';
        break;
      case "secondary":
        className += ' bg-white border-2 border-cyany';
        break;
    }

    return className;
  }
}
