import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss'],
})
export class SidebarComponent {
  @Output()
  logouted: EventEmitter<any> = new EventEmitter<any>();

  logout(): void {
    this.logouted.emit();
  }

  getCurrentUser(): string | null {
    return sessionStorage.getItem('user');
  }
}
