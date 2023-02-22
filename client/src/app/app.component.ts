import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  notes: any;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get('http://localhost:5273/api/note').subscribe({
      next: (data) => {
        this.notes = data;
        console.log(this.notes);
      },
      error: (error) => console.log(error),
      complete: () => console.log('Request completed!'),
    });
  }
}
