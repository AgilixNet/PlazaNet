import { createClient } from '@supabase/supabase-js'

const supabaseUrl = 'https://homkixaonxdourgskrwg.supabase.co'
const supabaseAnonKey = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImhvbWtpeGFvbnhkb3VyZ3NrcndnIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NjEzMzc3NzgsImV4cCI6MjA3NjkxMzc3OH0.clAzfnoncO7swFOLgOSfeTbn08zuBr596lkFQRz3bO0'

export const supabase = createClient(supabaseUrl, supabaseAnonKey)

