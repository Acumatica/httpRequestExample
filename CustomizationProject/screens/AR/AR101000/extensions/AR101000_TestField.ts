import { AR101000, ARSetup } from "src/screens/AR/AR101000/AR101000";
import {
 PXFieldState,
} from "client-controls";


export interface AR101000_TestField extends AR101000 {}
export class AR101000_TestField {}

export interface ARSetup_TestField extends ARSetup { }
export class ARSetup_TestField 
{
UsrTestField: PXFieldState;
}