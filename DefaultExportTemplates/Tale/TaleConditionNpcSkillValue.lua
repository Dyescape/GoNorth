{{- if condition.is_operator_primitive -}}
BaseNpc_GetSkillValue(this, "{{ condition.value_object.fields.ScriptName.value }}", "{{ condition.selected_field.name }}") {{ condition.operator }} {{ if condition.selected_field.type != "number"}}"{{ end }}{{ condition.compare_value }}{{ if condition.selected_field.type != "number"}}"{{ end }}
{{- end -}}
{{- if !condition.is_operator_primitive -}}
{{ condition.operator }}(BaseNpc_GetSkillValue(this, "{{ condition.value_object.fields.ScriptName.value }}", "{{ condition.selected_field.name }}") , "{{ condition.compare_value }}")
{{- end -}}