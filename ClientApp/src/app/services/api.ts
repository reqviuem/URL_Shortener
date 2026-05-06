import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ShortUrl {
  id: number;
  originalUrl: string;
  shortenedUrl: string;
  createdBy: string;
  createdDate: Date;
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = 'http://localhost:5068/api/url';

  constructor(private http: HttpClient) { }

  getAllUrls(): Observable<ShortUrl[]> {
    return this.http.get<ShortUrl[]>(this.apiUrl);
  }

  getUrlById(id: number): Observable<ShortUrl> {
    return this.http.get<ShortUrl>(`${this.apiUrl}/${id}`);
  }

  addUrl(originalUrl: string): Observable<ShortUrl> {
    return this.http.post<ShortUrl>(this.apiUrl, { originalUrl });
  }

  deleteUrl(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
