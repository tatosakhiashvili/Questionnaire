﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questionnaire.Domain {
	public class AnswerTemplate {
		public int Id { get; set; }
		public string Name { get; set; }
		public string FileName { get; set; }
		public string Body { get; set; }
	}
}
