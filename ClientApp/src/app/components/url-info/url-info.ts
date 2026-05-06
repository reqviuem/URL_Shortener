import { Component, OnInit,ChangeDetectorRef } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ApiService, ShortUrl } from '../../services/api';

@Component({
  selector: 'app-url-info',
  standalone: true,
  imports: [CommonModule, RouterLink, DatePipe],
  templateUrl: './url-info.html',
  styleUrl: './url-info.css'
})
export class UrlInfo implements OnInit {
  urlData: ShortUrl | null = null;
  errorMessage: string = '';
  loading: boolean = false;
  error: string = '';

  constructor(
    private route: ActivatedRoute,
    private apiService: ApiService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = parseInt(idParam, 10);
      this.fetchUrlInfo(id);
    }
  }

  fetchUrlInfo(id: number) {
    this.loading = true;
    this.cdr.detectChanges();

    this.apiService.getUrlById(id).subscribe({
      next: (data) => {
        this.urlData = data;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error fetching URL details:', err);
        this.error = 'Failed to load URL details. Please try again.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }
}
