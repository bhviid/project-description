/*
 * Copyright 2020 Arie van Deursen, TU Delft.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

public class DMM {

  public void hello() {
	a = 1;
	a = 1;
	a = 1;
	a = 1;
	a = 1;
	a = 1;
	a = 1;
	a = 1;
	a = 1;
	a = 1;
	a = 1;
	a = 1;
	a = 1;
	a = 1;
	a = 1;
	a = 1;
	a = 1;
	a = 1;
	a = 1;
	b = 2;
	d = 4;
  }

  /*
   * Unit interfacing off-point: risky
   */
  public void world(int a, int b, int c) {
	return a + b;
  }

  /*
   * Unit complexity off-point: risky
   */
  public String complexity(int m) {
  	switch (m) {
  		case 1: return "January"; break;
  		case 2: return "January"; break;
  		case 3: return "January"; break;
  		case 4: return "January"; break;
  		case 5: return "January"; break;
  		default: return null;
  	}
  }

  /*
   * Unit size off-point: risky
   */
  public int size() { // line 1
  	int line = 2;
  	line = 3;
 	line = 4;
  	line = 5;
  	line = 6;
  	line = 7;
  	line = 8;
  	line = 9;
  	line = 10;
  	line = 11;
  	line = 12;
  	line = 13;
  	line = 14;
  	line = 15;
  } // line  16

}
