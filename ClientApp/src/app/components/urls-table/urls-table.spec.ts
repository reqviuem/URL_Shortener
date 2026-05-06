import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UrlsTable } from './urls-table';

describe('UrlsTable', () => {
  let component: UrlsTable;
  let fixture: ComponentFixture<UrlsTable>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UrlsTable],
    }).compileComponents();

    fixture = TestBed.createComponent(UrlsTable);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
