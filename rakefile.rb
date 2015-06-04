
def read_version(file_name)
    version = '0.0.0.0'
    text = File.read(file_name)
    version = $1 if text =~ /Identity\sId=\".*?Version=\"(\d+[\.\d+]+)\"/
end

def make_msbuild_command_line(solution_file, target, configuration, platform)
  cmdline = "\"%VS120COMNTOOLS%\\..\\..\\VC\\vcvarsall.bat\" x86_amd64"
  cmdline += " & msbuild /m #{solution_file}"
  cmdline += " /t:#{target}"
  cmdline += " /p:Configuration=#{configuration}"
  cmdline += " /p:Platform=\"#{platform}\""
end

################################################################################

$project_name = 'BoostTestVSPackage'
$solution_file = 'BoostTestAddin.sln'
$staging_area_dir = 'staging_area'

$version = read_version('BoostTestVSPackage/source.extension.vsixmanifest')

$configuration = ENV['build_config'] || 'Release'
$platform = ENV['build_platform'] || 'Any CPU'
case $platform
when 'Any CPU'
    $build_output_dir = File.join($project_name, 'bin', $configuration)
when 'x86', 'x64'
    $build_output_dir = File.join($project_name,'bin', $platform, $configuration)
end

puts
puts "Building #{$solution_file}..."
puts "Configuration: #{$configuration}"
puts "Platform: #{$platform}"
puts "Version: #{$version}"
puts

################################################################################

task :default do
  puts `rake -T`
end

desc 'Build and rename the generated file for distribution'
task :distrib => [:rebuild] do
  output_file = File.join($build_output_dir, 'BoostTestVSPackage.vsix')
  distributed_name = "BoostTestVSPackage-#{$version}.vsix"
  FileUtils.copy_entry(output_file, distributed_name, :remove_destination => true)
  puts "Created #{distributed_name}"
end


desc 'Build whole solution'
task :build do |t|
  sh make_msbuild_command_line($solution_file, "Build", $configuration, $platform)
end

desc 'Clean whole solution'
task :clean do |t|
  sh make_msbuild_command_line($solution_file, "Clean", $configuration, $platform)
end

desc 'Run a clean build of the whole solution'
task :rebuild => [:clean, :build]
