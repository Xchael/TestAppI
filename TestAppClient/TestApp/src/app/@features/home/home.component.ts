import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { TableComponent } from '@components';
import { TestTable } from '@models';
import { TestTableDataService } from 'src/app/@data-service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  imports:[CommonModule, TableComponent],
  standalone: true
})
export class HomeComponent implements OnInit {

  tableData: TestTable[] = [];
  loading=true;
  error: string | null = null;

  constructor(private dataService: TestTableDataService) { }

  ngOnInit() {
    this.dataService.fetchTableData().subscribe({
      next: (data) => {
        this.tableData = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = err.message || 'Unknown error';
        this.loading=false;
      }
    });
  }
}
