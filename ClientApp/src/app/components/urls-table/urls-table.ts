import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ApiService, ShortUrl } from '../../services/api';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-urls-table',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './urls-table.html',
  styleUrl: './urls-table.css'
})
export class UrlsTable implements OnInit {
  urls: ShortUrl[] = [];
  newUrl: string = '';
  errorMessage: string = '';
  isLoggedIn: boolean = false;

  constructor(
    private apiService: ApiService,
    public authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.authService.isLoggedIn$.subscribe(status => {
      this.isLoggedIn = status;
      this.cdr.detectChanges();
    });

    this.loadUrls();
  }

  loadUrls() {
    this.apiService.getAllUrls().subscribe({
      next: (data: ShortUrl[]) => {
        this.urls = data;
        this.cdr.detectChanges();
      },
      error: (err: any) => console.error('Failed to load URLs', err)
    });
  }

  addUrl() {
    if (!this.newUrl) return;
    this.errorMessage = '';

    this.apiService.addUrl(this.newUrl).subscribe({
      next: (createdUrl: ShortUrl) => {
        this.urls.push(createdUrl);
        this.newUrl = '';
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        this.errorMessage = err.error?.message || 'Failed to add URL';
        this.cdr.detectChanges();
      }
    });
  }
  deleteUrl(id: number) {
    if (!confirm('Are you sure you want to delete this URL?')) return;

    this.apiService.deleteUrl(id).subscribe({
      next: () => {
        this.urls = this.urls.filter(u => u.id !== id);
        this.cdr.detectChanges();
      },
      error: (err: any) => alert('Failed to delete URL')
    });
  }

  canDelete(url: ShortUrl): boolean {
    if (!this.isLoggedIn) return false;

    const isAdmin = this.authService.isAdmin();
    const currentUsername = this.authService.getUsername();

    return isAdmin || (currentUsername === url.createdBy);
  }
}
