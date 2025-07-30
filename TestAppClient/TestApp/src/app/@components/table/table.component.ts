import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { TestTable } from '@models';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css'],
  imports:[CommonModule],
  standalone: true
})
export class TableComponent{
  @Input() data: TestTable[] = [];
}
