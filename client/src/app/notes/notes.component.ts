import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-notes',
  templateUrl: './notes.component.html',
  styleUrls: ['./notes.component.css'],
})
export class NotesComponent implements OnInit {
  notes: any = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get('http://localhost:5273/api/note/').subscribe({
      next: (data) => {
        this.notes = Object.values(data);
      },
      error: (error) => console.log(error),
    });
  }
}
