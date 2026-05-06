import { Component, OnInit } from '@angular/core';
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

  constructor(
    private route: ActivatedRoute,
    private apiService: ApiService
  ) {}

  ngOnInit() {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      const id = parseInt(idParam, 10);
      this.fetchUrlInfo(id);
    }
  }

  fetchUrlInfo(id: number) {
    this.apiService.getUrlById(id).subscribe({
      next: (data: ShortUrl) => this.urlData = data,
      error: (err: any) => this.errorMessage = 'Failed to load URL information.'
    });
  }
}
