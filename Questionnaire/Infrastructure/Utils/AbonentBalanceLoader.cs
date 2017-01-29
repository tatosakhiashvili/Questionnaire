using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Questionnaire.Infrastructure.Utils {
	public static class AbonentBalanceLoader {

		public static decimal GetBalance(string url, string mobileNo) {
			url = "http://10.10.25.226:8080/GeoCredit-0.0.1-SNAPSHOT/GeoCredit";
			try {
				var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.ContentType = "text/xml; charset=utf-8";
				httpWebRequest.Method = "POST";

				using(var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) {
					string json = @"<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" 
													xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
													xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" 
													xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"">
														<SOAP-ENV:Body>
															<getSubscrData xmlns=""http://ws.orga.geocredit.geocell.ge/"">
																<msisdn xmlns="""">995{0}</msisdn>
															</getSubscrData>
														</SOAP-ENV:Body>
												</SOAP-ENV:Envelope>";

					json = string.Format(json, mobileNo);

					streamWriter.Write(json);
					streamWriter.Flush();
					streamWriter.Close();
				}

				var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				using(var streamReader = new StreamReader(httpResponse.GetResponseStream())) {
					var result = streamReader.ReadToEnd();

					if(result.Contains("<debit>") && result.Contains("</debit>")) {
						int pFrom = result.IndexOf("<debit>") + "<debit>".Length;
						int pTo = result.LastIndexOf("</debit>");
						string _result = result.Substring(pFrom, pTo - pFrom);

						if(!string.IsNullOrEmpty(_result)) {
							decimal returnValue;
							if(decimal.TryParse(_result, out returnValue)) {
								return returnValue;
							}
						}
						return 0;
					} else {
						return 0;
					}
				}
			} catch(Exception) {
				return 0;
			}
		}
	}
}