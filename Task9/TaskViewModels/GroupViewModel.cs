﻿#nullable enable
using System.Collections.Generic;
using Services.ModelsDTO;

namespace Task9.TaskViewModels {
    public class GroupViewModel {
        public List<GroupDTO>? FilteredGroups { get; set; }
        public int? SelectedCourseId { get; set; }
    }
}
