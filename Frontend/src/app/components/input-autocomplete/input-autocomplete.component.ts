import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-input-autocomplete',
  templateUrl: './input-autocomplete.component.html',
  styleUrls: ['./input-autocomplete.component.scss'],
})
export class InputAutocompleteComponent<T extends { [key: string]: any }> {
  @Input() options: T[];
  @Input() property: keyof T;
  @Input() size: 'lg' | 'md' | 'sm' = 'lg';
  @Input() placeholder: string = 'Search';
  @Input() dropdown: boolean = false;
  @Output() filtered: EventEmitter<T[]> = new EventEmitter();
  @Output() selected: EventEmitter<T> = new EventEmitter();

  value: string = '';
  filteredOptions: T[];

  filterOptions() {
    this.filteredOptions = this.options.filter(
      (option) =>
        option[this.property]
          .toLowerCase()
          .indexOf(this.value.toLowerCase()) !== -1
    );
    this.filtered.emit(this.filteredOptions);
  }

  getClass(): string {
    let className: string =
      'bg-white focus:bg-gray-100 hover:bg-gray-50 transition-colors duration-300 focus:outline-none rounded-md text-gray border border-2 border-purple-600';

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

  selectEvent(item: T) {
    this.selected.emit(item);
  }
}
