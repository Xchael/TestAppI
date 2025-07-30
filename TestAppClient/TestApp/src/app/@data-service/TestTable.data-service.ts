import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { TestTable } from "@models";
import { Observable } from "rxjs";
import { environment } from "../environments/environment";

@Injectable({
    providedIn: 'root',
})
export class TestTableDataService{
    baseApiUrl: string = environment.baseApiURL;
    constructor(private _http:HttpClient){}

    fetchTableData(): Observable<TestTable[]>{
        return this._http.get<TestTable[]>(`${this.baseApiUrl}/api/TestApp/getAll`);
    }
}