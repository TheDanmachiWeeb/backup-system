import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-cat-page',
  templateUrl: './cat-page.component.html',
  styleUrls: ['./cat-page.component.scss'],
})
export class CatPageComponent implements OnInit {
  catImageUrl: string = '';

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.fetchRandomCatImage();
  }

  fetchRandomCatImage() {
    const apiUrl = 'https://api.thecatapi.com/v1/images/search';
    this.http.get<any[]>(apiUrl).subscribe((response: any[]) => {
      if (response.length > 0 && response[0].url) {
        this.catImageUrl = response[0].url;
      }
    });
  }
}
